import 'dart:io';
import 'package:get/get.dart';
import 'package:http/http.dart' as http;
import 'package:signalr_demo_app/services/auth_service.dart';
import 'localstorage_service.dart';

class AuthorizedClient extends http.BaseClient {
  final http.Client _inner;

  AuthorizedClient(this._inner);

  Future<http.StreamedResponse> send(http.BaseRequest request) async {
    var token = await LocalStorageService.get(LocalStorageService.JWT_KEY);
    //ensure token timeout / empty
    if (AuthService.IsJwtExpired(token)) {
      print("token is expired or empty, redirect to login page");
      Get.toNamed("/login");
    }

    request.headers[HttpHeaders.acceptHeader] = 'text/plain';
    request.headers[HttpHeaders.authorizationHeader] = 'Bearer ' + token;
    request.headers[HttpHeaders.contentTypeHeader] = 'application/json';
    return _inner.send(request);
  }
}
