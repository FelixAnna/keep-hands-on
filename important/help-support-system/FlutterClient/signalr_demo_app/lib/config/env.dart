import 'package:flutter_dotenv/flutter_dotenv.dart';

/// Interface for AppEnv. This will help us test our code.
mixin IAppEnv {
  late String hubApiAddress;
  late String userApiAddress;
  late String messageApiAddress;
  late String idpAuthority;
}

class AppEnv implements IAppEnv {
  final String hubApiAddress = dotenv.get('hubApiAddress');
  final String userApiAddress = dotenv.get('userApiAddress');
  final String messageApiAddress = dotenv.get('messageApiAddress');
  final String idpAuthority = dotenv.get('idpAuthority');
  final String name = 'pre';

  @override
  set hubApiAddress(String _hubApiAddress) {
    this.hubApiAddress = _hubApiAddress;
  }

  @override
  set userApiAddress(String _userApiAddress) {
    this.userApiAddress = _userApiAddress;
  }

  @override
  set messageApiAddress(String _messageApiAddress) {
    this.messageApiAddress = _messageApiAddress;
  }

  @override
  set idpAuthority(String _idpAuthority) {
    this.idpAuthority = _idpAuthority;
  }
}
