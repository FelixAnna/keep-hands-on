import 'dart:io';
import 'package:http/http.dart' as http;
import 'localstorage_service.dart';

class AuthorizedClient extends http.BaseClient {
  final http.Client _inner;

  AuthorizedClient(this._inner);

  Future<http.StreamedResponse> send(http.BaseRequest request) async {
    var tokenHeaderValue =
        'Bearer ' + await LocalStorageService.get(LocalStorageService.JWT_KEY);
    request.headers[HttpHeaders.acceptHeader] = 'text/plain';
    request.headers[HttpHeaders.authorizationHeader] = tokenHeaderValue;
    request.headers[HttpHeaders.contentTypeHeader] = 'application/json';
    return _inner.send(request);
  }
}
