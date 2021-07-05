'use strict';
const aws = require('aws-sdk');
const sharp = require('sharp');
const s3 = new aws.S3({ apiVersion: '2006-03-01' });
const dynamoClient = new aws.DynamoDB.DocumentClient();

const insertDynamoDB = async (dynamoParams) => {
  var params = {
    TableName: dynamoParams.dynamoTable,
    Item:{
        "s3bucket": dynamoParams.s3bucket,
        "s3key": dynamoParams.s3key,
        "info":{
            "content_type":  dynamoParams.content_type,
            "size": dynamoParams.size,
            "original_s3bucket": dynamoParams.source_bucket,
            "original_s3key": dynamoParams.source_key
        },
        "last_write_time": new Date().toJSON()
    }
  };

  console.log("Adding a new item...");
  await dynamoClient.put(params, function(err, data) {
      if (err) {
          console.error("Unable to add item. Error JSON:", JSON.stringify(err, null, 2));
      } else {
          console.log("Added item:", dynamoParams.s3key);
      }
  }).promise();
}

const updateDynamoDB = async (dynamoParams) => {
  var params = {
      TableName: dynamoParams.dynamoTable,
      Key:{
        "s3bucket": dynamoParams.s3bucket,
        "s3key": dynamoParams.s3key,
      },
      UpdateExpression: "set info.content_type =:content_type, info.size=:size, info.original_s3bucket=:o_s3bucket, info.original_s3key=:o_s3key, last_write_time=:last_write_time",
      ExpressionAttributeValues:{
          ":content_type": dynamoParams.content_type,
          ":size": dynamoParams.size,
          ":o_s3bucket": dynamoParams.source_bucket,
          ":o_s3key": dynamoParams.source_key,
          ":last_write_time": new Date().toJSON()
      },
      ReturnValues:"UPDATED_NEW"
  };

  console.log("Updating the item...");
  await dynamoClient.update(params, function(err, data) {
      if (err) {
          console.error("Unable to update item. Error JSON:", JSON.stringify(err, null, 2));
      } else {
          console.log("UpdateItem succeeded:", dynamoParams.s3key);
      }
  }).promise();
}

const upsertDynamoDB =async (dynamoParams)=>{
  var params = {
    TableName : dynamoParams.dynamoTable,
    KeyConditionExpression: "s3bucket =:s3bucket and s3key=:s3key",
    ExpressionAttributeValues: {
        ":s3bucket": dynamoParams.s3bucket,
        ":s3key": dynamoParams.s3key
    }
  };

  console.log("Find item:", dynamoParams.s3key);
  const results = await dynamoClient.query(params).promise();
  console.log("Found item:", results.Count);

  if(results.Count>0){
    await updateDynamoDB(dynamoParams);
  } else {
    await insertDynamoDB(dynamoParams);
  }
}


exports.handler = async (event, context) => {
    const bucket = event.Records[0].s3.bucket.name;
    const key = decodeURIComponent(event.Records[0].s3.object.key.replace(/\+/g, ' '));
    
    const destination_bucket = bucket;
    const destination_key = key.replace("original/", 'processed/');
    const dynamoTable ="photos-node";
    const imageType ="png";
    const params = {
        Bucket: bucket,
        Key: key,
    };
    try {
        const data = await s3.getObject(params).promise();
        console.log('CONTENT TYPE:', data.ContentType);
        if(data.ContentType !== 'image/png' && data.ContentType !== 'image/jpeg'){
          console.log('Unsupported file type');
          return data.ContentType;
        }
        
        const buffer = await sharp(data.Body)
              .resize(80, 100)
              .toFormat(imageType)
              .toBuffer();
        //console.log(buffer);
        //console.log(JSON.stringify(buffer, undefined, 2));

        await s3.putObject({
          Body: buffer,
          Bucket: destination_bucket,
          ContentType: data.ContentType,
          Key: destination_key,
        }).promise();

        const dynamoParams = {
          dynamoTable: dynamoTable,
          s3bucket: destination_bucket,
          s3key: destination_key,
          content_type: data.ContentType,
          size: data.ContentLength,
          source_bucket: bucket,
          source_key: key
        };
        await upsertDynamoDB(dynamoParams);

        return data.ContentType;
    } catch (err) {
        console.log(err);
        const message = `Error getting object ${key} from bucket ${bucket}. Make sure they exist and your bucket is in the same region as this function.`;
        console.log(message);
        throw new Error(message);
    }
}
