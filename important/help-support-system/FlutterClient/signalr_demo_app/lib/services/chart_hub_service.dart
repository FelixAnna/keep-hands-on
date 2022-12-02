import 'package:signalr_netcore/http_connection_options.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:signalr_netcore/itransport.dart';
import '../config/env.dart';
import 'localstorage_service.dart';

class HubService {
  static late HubConnection hubConnection;
  final IAppEnv env;
  HubService({required this.env});

  initHubConnection(
      {required Function(List<Object?>? parameters) listeningMessage,
      required String listenMethod}) async {
    final httpOptions = new HttpConnectionOptions(
        // logger: transportProtLogger,
        logMessageContent: true,
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: () =>
            LocalStorageService.get(LocalStorageService.JWT_KEY));
    hubConnection = HubConnectionBuilder()
        .withUrl(env.hubApiAddress + "/chat", options: httpOptions)
        .withAutomaticReconnect()
        .build();
    hubConnection.onclose(({Exception? error}) => print("Connection Closed"));
    hubConnection.on(listenMethod, listeningMessage);
  }

  startConnect({Function? callback}) async {
    if (hubConnection.state == HubConnectionState.Disconnected) {
      // once error go to login
      await hubConnection.start()?.catchError((onError) {
        callback?.call();
      });
    } else {
      await hubConnection.stop();
    }
  }
}
