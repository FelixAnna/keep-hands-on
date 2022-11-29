export const MathCategory = [
  { key: '0', text: '加法', value: 0 },
  { key: '1', text: '减法', value: 1 },
  { key: '2', text: '乘法', value: 2 },
  { key: '3', text: '除法', value: 3 },
];

export const MathKind = [
  {
    key: '1',
    text: '第一个数: ? + b= c ',
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

export const IORanges = [
  {
    value: 0,
    label: '-10^9',
    val: -1e9,
  },
  {
    value: 10,
    label: '-1000',
    val: -1000,
  },
  {
    value: 20,
    label: '-100',
    val: -100,
  },
  {
    value: 30,
    label: '0',
    val: 0,
  },
  {
    value: 40,
    label: '10',
    val: 10,
  },
  {
    value: 50,
    label: '20',
    val: 20,
  },
  {
    value: 60,
    label: '100',
    val: 100,
  },
  {
    value: 70,
    label: '1000',
    val: 1000,
  },
  {
    value: 80,
    label: '10^9',
    val: 1e9,
  },
];
