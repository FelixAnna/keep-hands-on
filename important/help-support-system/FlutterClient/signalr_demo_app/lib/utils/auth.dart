import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import 'config_util.dart';
import 'localstorage_util.dart';

class Auth {
  static Future<bool> login(username, password) async {
    var isSucceed = true;
    await getToken(username, password)
        .then((value) async => await LocalStorageUtil.save(LocalStorageUtil.JWT_KEY, value))
        .onError((error, stackTrace) => isSucceed = false);

    return isSucceed;
  }

  static Future<void> logout() async{
    await LocalStorageUtil.save(LocalStorageUtil.JWT_KEY, '');
  }

  static Future<String> getToken(username, password) async {
    final authServer = await loadConfig('authserver');

    Map data = {
      "userName": username,
      "password": password
    };
    var body = json.encode(data);
    final response = await http.post(
      Uri.parse(authServer + "/api/Users/login"),
      headers: {
        HttpHeaders.contentTypeHeader: 'application/json',
      },
      encoding: Encoding.getByName('utf-8'),
      body: body,
    );
    final responseJson = jsonDecode(response.body);
    if (response.statusCode == 200) {
      return responseJson['accessToken'].toString();
    }else{
      print('error' + response.toString());
    }
    return '';
  }
}
