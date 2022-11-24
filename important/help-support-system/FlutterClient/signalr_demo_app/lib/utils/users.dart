import 'dart:convert';
import 'dart:io';
import '../models/Friend.dart';
import 'config_util.dart';
import 'package:http/http.dart' as http;
import 'localstorage_util.dart';

class Users{
  static Future<List<Friend>> getCurrentUserFriends() async {
    final authServer = await loadConfig('authserver');

    final response = await http.get(
      Uri.parse(authServer + '/api/Users/contact'),
      headers: {
        HttpHeaders.acceptHeader: 'text/plain',
        HttpHeaders.authorizationHeader: 'Bearer ' + await LocalStorageUtil.get(LocalStorageUtil.JWT_KEY)
      },
    );
    var body = jsonDecode(response.body);

    var friendsJson = body['contact']['friends'] as List;
    List<Friend> friendsList = friendsJson.map((tagJson) => Friend.fromJson(tagJson)).toList();

    if (response.statusCode == 200) {
      return friendsList;
    }else{
      print('error' + response.toString());
    }
    return Future<List<Friend>>.value();
  }
}