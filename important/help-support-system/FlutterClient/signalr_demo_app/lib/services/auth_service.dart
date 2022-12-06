import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import 'package:jwt_decoder/jwt_decoder.dart';
import '../config/env.dart';
import '../utils/localstorage_service.dart';

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
                  LocalStorageService.PROFILE, jsonEncode(response["profile"])),
              await LocalStorageService.save(
                  LocalStorageService.REMEMBER_USERNAME, username)
            })
        .onError((error, stackTrace) => {isSucceed = false});

    return isSucceed;
  }

  Future<void> logout() async {
    await LocalStorageService.save(LocalStorageService.JWT_KEY, '');
    await LocalStorageService.save(LocalStorageService.PROFILE, '');
  }

  Future<dynamic> getToken(username, password) async {
    var body = json.encode({"userName": username, "password": password});
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

  static bool IsJwtExpired(String token) {
    if (token == '') {
      return true;
    }

    return JwtDecoder.isExpired(token);
  }
}
