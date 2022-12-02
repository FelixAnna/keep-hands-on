import 'dart:convert';
import 'dart:io';
import 'package:signalr_demo_app/models/group_member.dart';
import '../config/env.dart';
import '../models/contact.dart';
import 'package:http/http.dart' as http;
import '../utils/localstorage_service.dart';

class GroupMemberService {
  final IAppEnv env;
  GroupMemberService({required this.env});

  Future<GroupMembers> getGroupMembersInfo(groupId) async {
    final response = await http.get(
      Uri.parse(Uri.encodeFull(env.userApiAddress + '/api/groups/' + groupId)),
      headers: {
        HttpHeaders.acceptHeader: 'text/plain',
        HttpHeaders.authorizationHeader: 'Bearer ' +
            await LocalStorageService.get(LocalStorageService.JWT_KEY)
      },
    );
    var body = jsonDecode(response.body);

    var group = body['group'];
    GroupInfo groupInfo = GroupInfo.fromJson(body['group']);

    var membersJson = body['memebers'] as List;
    List<MemberInfo> members =
        membersJson.map((tagJson) => MemberInfo.fromJson(tagJson)).toList();

    if (response.statusCode == HttpStatus.ok) {
      return GroupMembers(members, group);
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }
}
