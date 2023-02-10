import * as signalR from '@microsoft/signalr';
import { ChatFrameScriptClass, MessageItemInterface, MessageItemTypeEnum } from "./chat-frame.interface";
import ChatFrameService from "./chat-frame.service";
import { generateCustomerItem, generateServiceItem } from './helper';

const HUB_API_SERVICE = process.env.HUB_API_SERVICE;

export default class ChatFrameScript extends ChatFrameScriptClass {
  private _service: ChatFrameService;
  private _connection: any;

  constructor(root: HTMLDivElement) {
    super();
    this._root = root;
    this._service = new ChatFrameService();

    const messages: MessageItemInterface[] = [];

    this.setData({
      messages,
    });
  }

  register() {
    console.log('register', this._root);
    this._render();
    this._connect();
  }

  unregister() {
    console.log('ChatFrameScript unregister')
  }

  private async _connect() {
    console.log('initial connection begin');

    const serviceInfo = await this._service.getService();
    console.log('service', serviceInfo);

    this.setData({
      accessToken: serviceInfo.accessToken,
      customerProfile: serviceInfo.profile,
      serviceProfile: serviceInfo.supportProfile,
      messages: [{
        type: MessageItemTypeEnum.service,
        receivedTime: Date.now(),
        content: 'Hi, what may I help you?',
      }]
    });

    await this._connectHubService(serviceInfo.accessToken);
  }

  private _renderMessageList() {
    this.observeData('messages').subscribe((data: MessageItemInterface[]) => {
      console.log('rendering messages list', data);

      Array.from(this._nodes.list.children).forEach((item) => {
        if (item.nodeType === 1) {
          const hold = this.data.messages.some((message: MessageItemInterface) => {
            (String(message.receivedTime) === (<HTMLLIElement>item).dataset.timestamp)
          });
          if (!hold) {
            this._nodes.list.removeChild(item);
          }
        }
      });

      let increasedItem: MessageItemInterface[];
      const lastListItem = (<HTMLElement>this._nodes.list.lastElementChild);
      if (!lastListItem) {
        increasedItem = data;
      } else {
        const timstamp = lastListItem.dataset.timestamp;
        const lastItemIndex = data.findIndex((item) => String(item.receivedTime) === timstamp);
        increasedItem = data.slice(lastItemIndex);
      }

      const increasedHtml = increasedItem.reduce((acc: string, item: MessageItemInterface) => {
        let htmlString: string
        if (item.type === MessageItemTypeEnum.customer) {
          htmlString = generateCustomerItem({
            name: this.data?.customerProfile?.nickName,
            receivedTime: item.receivedTime,
            content: item.content,
            avartar: this.data?.customerProfile?.avatarUrl,
            status: item.status,
          });
        } else if (item.type === MessageItemTypeEnum.service) {
          htmlString = generateServiceItem({
            name: this.data?.serviceProfile?.nickName,
            content: item.content,
            receivedTime: item.receivedTime,
            avartar: this.data?.serviceProfile?.avatarUrl,
          });
        } else {
          htmlString = '';
        }
        const newAcc = acc + htmlString;
        return newAcc;
      }, '');
      const template = document.createElement('template');
      template.innerHTML = increasedHtml;
      const frag = document.createDocumentFragment();
      frag.appendChild(template.content);
      this._nodes.list.appendChild(frag);

      this._showLatest();
      this._clearEditor();
      console.log('rendered message list');
    });
  }

  private async _sendMessage(data: string) {
    console.log('begin to send message, data:', data);
    let sentStatus = false;
    if (this._connection.state === signalR.HubConnectionState.Connected) {
      try {
        const userId = this.data.serviceProfile.userId;
        console.log('user id:', userId);
        await this._connection.invoke('SendToUser', userId, data);
        sentStatus = true;
      } catch (error) {
        console.error('sending message error: ', error);
      }
    }
    console.log('sent message', sentStatus ? 'successfully' : 'failure');
    return sentStatus;
  }

  private async _connectHubService(accessToken: string) {
    console.log('connect hub service begin, ', HUB_API_SERVICE);
    const options = {
      accessTokenFactory: () => Promise.resolve(accessToken),
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
    };
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_API_SERVICE, options)
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();
    try {
      this._connection.start();
      this._connection.on('ReceiveMessage', (sender: string, _user: string, message: string) => {
        console.log('received message ', message, ' from ', sender);
        const newMessageItem: MessageItemInterface = {
          type: MessageItemTypeEnum.service,
          receivedTime: Date.now(),
          content: message,
          status: true,
        };
        this.setData({
          messages: [...this.data.messages, newMessageItem],
        });
      });

      // this._connection.onreconnecting((error) => {
      //   console.log('reconnecting hub service');
      // });

      // this._connection.onreconnected((connectionId) => {
      //   console.log('reconnected hub service ', connectionId);
      // });
    } catch (error) {
      console.log('connect hub service error', error);
    }
    console.log('connection state: ', this._connection.state);
    console.log('connect hub service end');
  }

  private _render() {
    console.log('render ui begin');

    this._obtainNodes();

    this._bindEvents();

    this._renderMessageList();

    console.log('render ui end');
  }

  private _bindEvents() {
    this._bindEditorEvent();
    this._bindSendButtonEvent();
    this._bindCloseEvent();

    this._nodes.list.addEventListener('click', async (e) => {
      const target = <HTMLElement>e.target;
      if ((<HTMLElement>target.parentNode).classList.contains('retry-button')) {
        const parent = <HTMLElement>target.parentNode;
        const id = parent.getAttribute('id');
        const needRetryIndex: number = this.data.messages.findIndex(
          (item: MessageItemInterface) => id === String(item.receivedTime)
        );
        const needRetryItem = this.data.messages[needRetryIndex];
        const sentStatus = await this._sendMessage(needRetryItem.content);
        needRetryItem.status = sentStatus;
        const newMessagesLeft = this.data.messages.slice(0, needRetryIndex);
        const newMessagesRight = this.data.messages.slice(needRetryIndex + 1);
        this.setData({
          messages: [...newMessagesLeft, ...newMessagesRight, needRetryItem],
        });
      }
    });
  }

  private _bindEditorEvent() {
    const editor = this._nodes.footer.children[0];
    editor.addEventListener('keydown', (e: KeyboardEvent) => {
      if (e.altKey && e.key.toLowerCase() === 'enter') {
        e.preventDefault();
        console.log('alt + enter pressed');
        const newValue = (<HTMLInputElement>e.target).value;
        const newMessageItem: MessageItemInterface = {
          type: MessageItemTypeEnum.customer,
          receivedTime: Date.now(),
          content: newValue,
          status: false,
        };
        this.setData({
          messages: [...this.data.messages, newMessageItem],
        });
      }
    });
  }

  private _bindSendButtonEvent() {
    const sendButton = this._nodes.footer.children[1];
    sendButton.addEventListener('click', async (e) => {
      console.log('send button clicked');
      e.preventDefault();
      const editorValue = (<HTMLInputElement>this._nodes.footer.children[0]).value;
      const sentStatus = await this._sendMessage(editorValue);
      const newMessageItem: MessageItemInterface = {
        type: MessageItemTypeEnum.customer,
        receivedTime: Date.now(),
        content: editorValue,
        status: sentStatus,
      };
      this.setData({
        messages: [...this.data.messages, newMessageItem],
      });
    });
  }

  private _bindCloseEvent() {
    const closeButton = this._nodes.header.children[1];
    closeButton.addEventListener('click', (e: MouseEvent) => {
      console.log('close window');
      e.preventDefault();
      this._root.style.visibility = 'hidden';
      // window.sessionStorage.setItem('current-chat-cache', JSON.stringify(this.data.messages));
    });
  }

  private _obtainNodes() {
    this._nodes.header = this._root.querySelector('#header');
    this._nodes.body = this._root.querySelector('#body');
    this._nodes.list = <HTMLElement>this._nodes.body.children[0];
    this._nodes.footer = this._root.querySelector('#footer');
  }

  private _showLatest() {
    this._nodes.body.scrollTop = this._nodes.body.scrollHeight;
  }

  private _clearEditor() {
    const editor = this._nodes.footer.children[0];
    (<HTMLTextAreaElement>editor).value = '';
    this.setData({
      editorValue: '',
    });
  }
}