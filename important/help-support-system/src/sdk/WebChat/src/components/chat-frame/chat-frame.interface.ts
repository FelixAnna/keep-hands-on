import { Subject } from "rxjs";

const needObserveProps = {
  messages: true,
};

export abstract class ChatFrameScriptClass {
  protected _root: HTMLDivElement;

  protected _nodes: Record<string, HTMLElement>;

  private _data: Record<string, any>;

  private _subjects: Record<symbol, Subject<any>>;

  constructor() {
    const _subjects = {};
    this._data = new Proxy({}, {
      set(target: Record<string, any>, prop: string, value: any) {
        if (needObserveProps[prop]) {
          if (!_subjects[Symbol.for(prop)]) {
            _subjects[Symbol.for(prop)] = new Subject();
          }
          _subjects[Symbol.for(prop)].next(value);
        }
        return Reflect.set(target, prop, value);
      }
    });
    this._subjects = _subjects;
    this._nodes = {};
  }

  protected setData(value: Record<string, any>): boolean {
    let result: boolean;
    if (Object.prototype.toString.call(value) !== '[object Object]') {
      result = false;
    } else {
      Object.entries(value).forEach(([key, value]) => {
        this._data[key] = value;
      })
      result = true;
    }

    return result;
  }

  protected observeData(prop: string) {
    let result: Subject<any>;
    if (needObserveProps[prop]) {
      if (!this._subjects[Symbol.for(prop)]) {
        this._subjects[Symbol.for(prop)] = new Subject();
      }
      result = this._subjects[Symbol.for(prop)];
    } else {
      result = null;
    }
    return result;
  }

  protected get data() {
    return this._data;
  }

  protected set data(value: any) {
    throw new Error(`Don\'t support to set ${value.toString()} to data directly!`);
  }
}

export enum MessageItemTypeEnum {
  customer,
  service,
}

export interface MessageItemInterface {
  type: MessageItemTypeEnum;
  receivedTime: string | Date | number;
  content: string;
  status: boolean;
}