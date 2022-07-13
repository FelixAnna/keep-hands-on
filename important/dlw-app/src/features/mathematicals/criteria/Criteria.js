import React from 'react';
import PropTypes from 'prop-types';
import { MathCategory, MathKind, MathType } from '../const';

class Criteria extends React.Component {
  convertToText = (value, type) => { // eslint-disable-line
    let result = '';
    switch (type) {
      case 'Category':
        result = MathCategory.find((op) => op.value === value).text;
        break;
      case 'Kind':
        result = MathKind.find((op) => op.value === value).text;
        break;
      case 'Type':
        MathType.forEach((group) => {
          const option = group.options.find((op) => op.value === value);
          if (option !== undefined) {
            result = option.text;
          }
        });
        break;

      default:
        break;
    }

    return result;
  };

  render() {
    const { data } = this.props;
    return (
      <div key={data.index}>
        <span>
          {data.index + 1}
          .
        </span>
        <span>
          {data.Min}
          ~
          {data.Max}
        </span>
        <span>
          {data.Range.Min}
          ~
          {data.Range.Max}
        </span>
        <span>{data.Quantity}</span>
        <span>{this.convertToText(data.Category, 'Category')}</span>
        <span>{this.convertToText(data.Kind, 'Kind')}</span>
        <span>{this.convertToText(data.Type, 'Type')}</span>
      </div>
    );
  }
}

Criteria.propTypes = {
  data: PropTypes.shape({
    index: PropTypes.number,
    Min: PropTypes.number,
    Max: PropTypes.number,
    Range: PropTypes.shape({
      Min: PropTypes.number,
      Max: PropTypes.number,
    }),
    Quantity: PropTypes.number,
    Category: PropTypes.number,
    Kind: PropTypes.number,
    Type: PropTypes.number,
  }).isRequired,
};
export default Criteria;
