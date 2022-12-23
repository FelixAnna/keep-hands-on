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

  Future<void> logout() async {
    await LocalStorageService.save(LocalStorageService.JWT_KEY, '');
    await LocalStorageService.save(LocalStorageService.PROFILE, '');
  }

  Future<dynamic> login(username, password) async {
    var body = json.encode({"userName": username, "password": password});
    var start = DateTime.now().millisecondsSinceEpoch;
    final response = await http.post(
      Uri.parse(env.userApiAddress + "/api/auth/login"),
      headers: {
        HttpHeaders.contentTypeHeader: 'application/json',
      },
      encoding: Encoding.getByName('utf-8'),
      body: body,
    );
    final responseJson = jsonDecode(response.body);
    print(DateTime.now().millisecondsSinceEpoch - start);
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
