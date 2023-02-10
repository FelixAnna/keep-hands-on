export interface ChatButtonInterface extends HTMLElement {
  onClick?: () => void;
}

export abstract class ChatButtonClass extends HTMLElement {
  protected _onClick?: () => void;
  public set onClick(value: () => void) {
    this._onClick = value;
  }
}