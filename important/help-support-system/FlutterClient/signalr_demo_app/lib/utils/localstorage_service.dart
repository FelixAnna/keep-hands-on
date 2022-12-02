import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class LocalStorageService {
  static const JWT_KEY = "jwt";
  static const PROFILE = "profile";
  static const REMEMBER_USERNAME = 'curr_username';
  static final storage = FlutterSecureStorage();

  static Future<void> save(key, value) async {
    await storage.write(key: key, value: value);
  }

  static Future<String> get(key) async {
    if (await storage.containsKey(key: key)) {
      var res = await storage.read(key: key);
      return res.toString();
    }

    return '';
  }
}
