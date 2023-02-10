import { ChatButtonInterface } from './components/chat-button/chat-button.interface';
import ChatButton from "./components/chat-button/chat-button";
import ChatFrame from "./components/chat-frame/chat-frame";

export default {
}

console.log('current time', new Date().toLocaleString());

customElements.define('chat-button', ChatButton);
customElements.define('chat-frame', ChatFrame);

window.addEventListener('DOMContentLoaded', () => {
  console.log('DOMContentLoaded');
  const chatButton: ChatButtonInterface = document.createElement('chat-button');
  chatButton.onClick = () => {
    console.log('chat button onClick invoked');
    const chatFrameElement = document.getElementsByTagName('chat-frame')[0];
    if (!chatFrameElement) {
      document.body.appendChild(document.createElement('chat-frame'));
    } else {
      (<HTMLDivElement>chatFrameElement.shadowRoot.children[1])!.style.visibility = 'visible';
    }
  };

  document.body.appendChild(chatButton);
  console.log('chat button connected', chatButton.isConnected);
});