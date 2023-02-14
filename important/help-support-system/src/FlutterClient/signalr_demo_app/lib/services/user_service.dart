import 'dart:convert';
import 'dart:io';
import '../config/env.dart';
import '../models/contact.dart';
import 'package:http/http.dart' as http;

class UserService {
  final IAppEnv env;
  final http.Client client;
  UserService(this.client, {required this.env});

  Future<Contact> getCurrentContacts() async {
    final response =
        await client.get(Uri.parse(env.userApiAddress + '/api/users/contact'));
    var body = jsonDecode(response.body);

    var friendsJson = body['contact']['friends'] as List;
    List<MemberInfo> friendsList =
        friendsJson.map((tagJson) => MemberInfo.fromJson(tagJson)).toList();

    var groupsJson = body['contact']['groups'] as List;
    List<GroupInfo> groupsList =
        groupsJson.map((tagJson) => GroupInfo.fromJson(tagJson)).toList();

    if (response.statusCode == HttpStatus.ok) {
      return Contact(body['userId'], groupsList, friendsList);
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }

  Future<Contact> getCurrentMessengers() async {
    final response = await client
        .get(Uri.parse(env.userApiAddress + '/api/users/messengers'));
    var body = jsonDecode(response.body);

    var friendsJson = body['contact']['friends'] as List;
    List<MemberInfo> friendsList =
        friendsJson.map((tagJson) => MemberInfo.fromJson(tagJson)).toList();

    var groupsJson = body['contact']['groups'] as List;
    List<GroupInfo> groupsList =
        groupsJson.map((tagJson) => GroupInfo.fromJson(tagJson)).toList();

    if (response.statusCode == HttpStatus.ok) {
      return Contact(body['userId'], groupsList, friendsList);
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }

  Future<List<ColleagueInfo>> getColleagues(String keywords) async {
    var query = "";
    if (keywords.trim().length > 0) {
      query = '?keywords=' + jsonEncode(keywords.trim());
    }
    final response = await client
        .get(Uri.parse(env.userApiAddress + '/api/users/colleagues' + query));
    var body = jsonDecode(response.body);

    var colleaguesJson = body['colleagues'] as List;
    List<ColleagueInfo> colleaguesList = colleaguesJson
        .map((tagJson) => ColleagueInfo.fromJson(tagJson))
        .toList();

    if (response.statusCode == HttpStatus.ok) {
      return colleaguesList;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }
}
