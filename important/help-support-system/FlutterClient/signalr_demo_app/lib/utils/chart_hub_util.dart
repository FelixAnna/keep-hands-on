import 'package:signalr_netcore/http_connection_options.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:signalr_netcore/itransport.dart';
import 'config_util.dart';
import 'localstorage_util.dart';

class ChatHubUtil {
  static late HubConnection hubConnection;
  static late String serverUrl;

  static initHubConnection({required Function(List<Object?>? parameters) listeningMessage}) async {
    final httpOptions = new HttpConnectionOptions(
        // logger: transportProtLogger,
        logMessageContent: true,
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets ,
        accessTokenFactory: () => LocalStorageUtil.get(LocalStorageUtil.JWT_KEY)
    );
    serverUrl = await loadConfig('hubserver');
    hubConnection = HubConnectionBuilder().withUrl(serverUrl, options: httpOptions).build();
    hubConnection.onclose(({Exception? error}) => print("Connection Closed"));
    hubConnection.on("ReceiveMessage", listeningMessage);
  }

  static startConnect ({Function? callback}) async{
    if(hubConnection.state == HubConnectionState.Disconnected){
      // once error go to login
      await hubConnection.start()?.catchError((onError) {
        callback?.call();
      }
      );
    }else{
      await hubConnection.stop();
    }
  }
}