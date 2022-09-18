export const MathCategory = [
  { key: '0', text: '加法', value: 0 },
  { key: '1', text: '减法', value: 1 },
];

export const MathKind = [
  {
    key: '1',
    text: '第一个数：? + b= c ',
    symble: '? + b= c',
    value: 1,
  },
  {
    key: '2',
    text: '第二个数: a + ?= c ',
    symble: 'a + ?= c',
    value: 2,
  },
  {
    key: '3',
    text: '结果: a + b= ? ',
    symble: 'a + b= ?',
    value: 3,
  },
];

export const MathType = [
  {
    text: '算术表达式',
    key: 1,
    options: [
      { key: '0', text: 'a+b=c', value: 0 },
    ],
  },
  {
    text: '应用题(比数字)',
    key: 2,
    options: [
      { key: '1', text: '比a多b的数是c', value: 1 },
    ],
  },
  {
    text: '应用题(比苹果)',
    key: 3,
    options: [
      { key: '2', text: '小明有a个苹果，小红比小明多b个，小红有c个苹果？', value: 2 },
    ],
  },
  {
    text: '应用题(随机)',
    key: 4,
    options: [
      { key: '3', text: '哥哥身高a厘米，妹妹比哥哥矮b厘米，妹妹身高c厘米？', value: 3 },
    ],
  },
];

export const RangeNumber = [
  {
    key: 1,
    text: '10 以内',
    data: {
      min: 0,
      max: 10,
    },
  },
  {
    key: 2,
    text: '20 以内',
    data: {
      min: 0,
      max: 20,
    },
  },
  {
    key: 3,
    text: '100 以内',
    data: {
      min: 0,
      max: 100,
    },
  },
  {
    key: 4,
    text: '1000 以内',
    data: {
      min: 0,
      max: 100,
    },
  },
  {
    key: 5,
    text: '任意数',
    data: {
      min: -10000000000,
      max: 10000000000,
    },
  },
];
