import { ChatButtonClass } from './chat-button.interface';
import htmlTemplate from 'bundle-text:./chat-button.html';

export default class ChatButton extends ChatButtonClass {
  constructor() {
    super();

    const shadow = this.attachShadow({ mode: 'open' });

    const template = document.createElement('template');
    template.innerHTML = htmlTemplate;
    shadow.appendChild(template.content);
  }

  connectedCallback() {
    this.shadowRoot.addEventListener('click', this._onClick);
  }

  disconnectedCallback() {
    this.shadowRoot.removeEventListener('click', this._onClick);
  }
}