import ChatFrameScript from "./chat-frame.script";
import styleTemplate from 'bundle-text:./chat-frame.css';
import htmlTemplate from 'bundle-text:./chat-frame.html';

export default class ChatFrame extends HTMLElement {
  private _script = null;

  // static get observedAttributes() {
  //   return [];
  // }

  constructor() {
    super();

    const shadow = this.attachShadow({mode: 'open'});

    const root = document.createElement('div');
    root.setAttribute('id', 'root');
    root.innerHTML = htmlTemplate;

    const style = document.createElement('style');
    style.textContent = styleTemplate;

    shadow.appendChild(style);
    shadow.appendChild(root);

    this._script = new ChatFrameScript(root);
  }

  connectedCallback() {
    console.log('Custom square element added to page.');
    this._script.register();
  }

  disconnectedCallback() {
    console.log('Custom square element removed from page.');
    this._script.unregister();
  }

  // adoptedCallback() {
  //   console.log('Custom square element moved to new page.');
  // }

  // attributeChangedCallback(name, oldValue, newValue) {
  //   console.log('Custom square element attributes changed.');
  // }
  
}