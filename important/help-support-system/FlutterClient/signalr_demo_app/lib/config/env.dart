import 'package:flutter_dotenv/flutter_dotenv.dart';

/// Interface for AppEnv. This will help us test our code.
mixin IAppEnv {
  late String hubApiAddress;
  late String userApiAddress;
}

class AppEnv implements IAppEnv {
  final String hubApiAddress = dotenv.get('hubApiAddress');
  final String userApiAddress = dotenv.get('userApiAddress');
  final String name = 'pre';

  @override
  set hubApiAddress(String _hubApiAddress) {
    this.hubApiAddress = _hubApiAddress;
  }

  @override
  set userApiAddress(String _userApiAddress) {
    this.userApiAddress = _userApiAddress;
  }
}
