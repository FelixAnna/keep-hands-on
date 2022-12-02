import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import '../config/env.dart';
import 'localstorage_service.dart';

class AuthService {
  final IAppEnv env;

  AuthService({required this.env});

  Future<bool> login(username, password) async {
    var isSucceed = true;
    await getToken(username, password)
        .then((response) async => {
              await LocalStorageService.save(
                  LocalStorageService.JWT_KEY, response["accessToken"]),
              await LocalStorageService.save(
                  LocalStorageService.PROFILE, jsonEncode(response["profile"]))
            })
        .onError((error, stackTrace) => {isSucceed = false});

    return isSucceed;
  }

  Future<void> logout() async {
    await LocalStorageService.save(LocalStorageService.JWT_KEY, '');
    await LocalStorageService.save(LocalStorageService.PROFILE, '');
  }

  Future<dynamic> getToken(username, password) async {
    Map data = {"userName": username, "password": password};
    var body = json.encode(data);
    final response = await http.post(
      Uri.parse(env.userApiAddress + "/api/users/login"),
      headers: {
        HttpHeaders.contentTypeHeader: 'application/json',
      },
      encoding: Encoding.getByName('utf-8'),
      body: body,
    );
    final responseJson = jsonDecode(response.body);
    if (response.statusCode == HttpStatus.ok) {
      return responseJson;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }
}
