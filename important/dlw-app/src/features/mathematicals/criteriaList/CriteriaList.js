import React from 'react';
import { useSelector} from 'react-redux';
import { DataGrid } from '@mui/x-data-grid';
import {MathCategory, MathKind, MathType} from '../const'
import {currentCriterias} from '../reducers/searchBar';

const convertToText = (value, type) => {
    let result =""
     switch (type) {
         case "Category":
             result = MathCategory.find(op=> op.value === value).text
             break;
         case "Kind":
             result = MathKind.find(op=> op.value === value).text
             break;
         case "Type":
            MathType.forEach(group=> {
                const option = group.options.find(op=>op.value === value)
                if(option !== undefined){
                    result = option.text
                }
            })
             break;
     
         default:
             break;
     }

     return result
 }
const columns = [
    { field: 'id', headerName: 'ID', width: 70},
    { field: 'inputRange', headerName: '数字范围', width: 100,
        valueGetter: (params) =>
        `${params.row.Min || ''} ${params.row.Max || ''}`
    },
    { field: 'outputRange', headerName: '结果范围', width: 100,
        valueGetter: (params) =>
        `${params.row.Range.Min || ''} ${params.row.Range.Max || ''}` },
    { field: 'Quantity', headerName: '题目数量', width: 70 },
    { field: 'Category', headerName: '算术类型', width: 70,
    valueGetter: (params) =>
    `${convertToText(params.row.Category,"Category")}` },
    { field: 'Kind', headerName: '求值类型', width: 150,
    valueGetter: (params) =>
    `${convertToText(params.row.Kind,"Kind")}` },
    { field: 'Type', headerName: '输出格式', width: 220,
    valueGetter: (params) =>
    `${convertToText(params.row.Type,"Type")}` },
]


function CriteriaList(){
    const criterias = useSelector(currentCriterias)
    const rows = criterias.map((q,i) => { return {id: i+1, ...q}})

    return (
        <div style={{ height: '375px', width: '100%' }}>
            <DataGrid
                rows={rows}
                columns={columns}
                pageSize={5}
                rowsPerPageOptions={[5]}
                checkboxSelection
            />
        </div>
        )
}

export default CriteriaList;