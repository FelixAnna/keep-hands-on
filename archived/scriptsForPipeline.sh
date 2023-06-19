## read file content from properties, save to dictionary
#!/bin/bash

# Define the properties file path
PROPERTIES_FILE="example.properties"

# Declare an associative array to store the key-value pairs
declare -A properties

# Read the properties file line by line
while IFS='=' read -r key value; do
  # Remove leading/trailing whitespace from the key and value
  key=$(echo "$key" | tr -d '[:space:]')
  value=$(echo "$value" | tr -d '[:space:]')
  # Add the key-value pair to the dictionary
  properties["$key"]="$value"
done < "$PROPERTIES_FILE"

# Print the dictionary
for key in "${!properties[@]}"; do
  echo "Key: $key, Value: ${properties[$key]}"
done


## read file content to dictionary with value split by ","

#!/bin/bash

# Define the properties file path
PROPERTIES_FILE="example.properties"

# Declare an associative array to store the key-value pairs
declare -A properties

# Read the properties file line by line
while IFS='=' read -r key value; do
  # Remove leading/trailing whitespace from the key and value
  key=$(echo "$key" | tr -d '[:space:]')
  value=$(echo "$value" | tr -d '[:space:]')
  # Split the value by comma and add each item to the dictionary
  IFS=',' read -ra items <<< "$value"
  for item in "${items[@]}"; do
    properties["$key"]+=" $item"
  done
done < "$PROPERTIES_FILE"

# Print the dictionary
for key in "${!properties[@]}"; do
  echo "Key: $key, Value: ${properties[$key]}"
done

## find common keys in both dictionary
#!/bin/bash

# Define the first dictionary
declare -A dict1=(
  ["key1"]="value1"
  ["key2"]="value2"
  ["key3"]="value3"
)

# Define the second dictionary
declare -A dict2=(
  ["key2"]="value2"
  ["key4"]="value4"
)

# Loop through the keys of the first dictionary
for key in "${!dict1[@]}"; do
  # Check if the key is in the second dictionary
  if [[ -v dict2["$key"] ]]; then
    # Print the key and value of both dictionaries
    echo "Key: $key, Value1: ${dict1[$key]}, Value2: ${dict2[$key]}"
  else
    # Key is not in second dictionary, continue to next key
    continue
  fi
done
