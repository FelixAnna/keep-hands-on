import 'package:signalr_netcore/http_connection_options.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:signalr_netcore/itransport.dart';
import '../config/env.dart';
import '../utils/localstorage_service.dart';

class HubService {
  static HubConnection? hubConnection = null;
  final IAppEnv env;
  HubService({required this.env});

  initial() async {
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
    hubConnection!.onreconnected(({connectionId}) {
      print("Reconnected");
    });
    hubConnection!.onreconnecting(({error}) {
      print("Reconnecting ... ");
    });
    hubConnection!.onclose(({Exception? error}) => print("Connection Closed"));
  }

  subscribe({
    required Function(List<Object?>? parameters) listeningMessage,
    required String listenMethod,
  }) {
    hubConnection!.on(listenMethod, listeningMessage);
  }

  start({Function? callback}) async {
    if (hubConnection!.state == HubConnectionState.Disconnected) {
      // once error go to login
      await hubConnection!
          .start()
          ?.then((x) => {print("Connection Started")})
          .catchError((onError) {
        callback?.call();
      });
    }
  }

  stop() async {
    if (hubConnection != null &&
        hubConnection!.state == HubConnectionState.Connected) {
      await hubConnection!.stop();
    }
  }
}
