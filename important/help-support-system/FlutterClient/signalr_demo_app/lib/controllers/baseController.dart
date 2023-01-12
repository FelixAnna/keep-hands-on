import 'dart:convert';

import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/chatDetailController.dart';
import 'package:signalr_demo_app/controllers/groupChatDetailController.dart';

import '../models/user.dart';
import '../services/auth_service.dart';
import '../services/auth_storage_service.dart';
import '../services/hub_service.dart';
import 'chatContainerController.dart';

class BaseController extends GetxController {
  late String UserName = "";
  late String Token = "";
  late User Profile = User(UserId: '', NickName: '', AvatarUrl: '', Email: '');

  late AuthStorageService authStorageService;
  @override
  void onInit() async {
    super.onInit();
  }

  Future<bool> loadUserFromCache() async {
    authStorageService = Get.find<AuthStorageService>();

    // load last login info from local cache
    var loginCache = await authStorageService.GetLoginCache();
    UserName = loginCache[2];

    //if token expired: clear token and ensure logout
    if (AuthService.IsJwtExpired(loginCache[0])) {
      print("token expired");
      Token = '';
      await logout();
      return false;
    }

    print("token valid");
    //if already logged in and valid, do something else
    Token = loginCache[0];
    Profile = User.fromJson(jsonDecode(loginCache[1]));

    return true;
  }

  logout() async {
    var hubService = Get.find<HubService>();

    var chatContainer = Get.find<ChatContainerController>();
    for (var element in chatContainer.groupFollowers) {
      await Get.delete<GroupChatDetailController>(tag: element.chatId);
    }
    chatContainer.groupFollowers.clear();
    for (var element in chatContainer.personFollowers) {
      await Get.delete<ChatDetailController>(tag: element.chatId);
    }
    chatContainer.personFollowers.clear();

    await authStorageService.ClearLoginResult();
    await hubService.stop();
  }
}
