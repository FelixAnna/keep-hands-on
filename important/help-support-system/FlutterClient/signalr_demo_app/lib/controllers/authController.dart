import 'dart:convert';

import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/config/env.dart';
import 'package:signalr_demo_app/models/login_response.dart';
import 'package:signalr_demo_app/services/auth_service.dart';
import '../services/auth_storage_service.dart';

class AuthController extends GetxController {
  late String Token;
  late User Profile;
  late String IdpAuthority;

  var UserNameEditor = TextEditingController();
  var PasswordEditor = TextEditingController();

  late AuthStorageService authStorageService;

  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    authStorageService = Get.find<AuthStorageService>();
    IdpAuthority = Get.find<IAppEnv>().idpAuthority;

    // load last login info
    var loginCache = await authStorageService.GetLoginCache();
    UserNameEditor.text = loginCache[2];

    //clear storage if token expired
    if (AuthService.IsJwtExpired(loginCache[0])) {
      print("token expired, and will be clear");
      await logout();
      Token = '';
      return;
    }

    print("token valid");
    //if already logged in and valid, do something else
    Token = loginCache[0];
    Profile = User.fromJson(jsonDecode(loginCache[1]));
  }

  isLoggedIn() {
    return Token.length > 0 && Profile.UserId != '';
  }

  login(bool keepUserName) async {
    await Get.find<AuthService>()
        .getToken(UserNameEditor.text, PasswordEditor.text)
        .then((response) async => {
              Token = response["accessToken"],
              Profile = User.fromJson(response["profile"]),
              await authStorageService.SaveLoginResult(response, keepUserName),
            })
        .onError((error, stackTrace) => {
              Token = '',
            });
    update();
  }

  logout() async {
    await authStorageService.ClearLoginResult();
  }
}
