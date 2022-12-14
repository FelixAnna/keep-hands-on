import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/config/env.dart';
import 'package:signalr_demo_app/controllers/profileController.dart';
import 'package:signalr_demo_app/models/login_response.dart';
import 'package:signalr_demo_app/services/auth_service.dart';
import '../services/auth_storage_service.dart';
import '../services/hub_service.dart';

class AuthController extends ProfileController {
  late String IdpAuthority = '';

  var UserNameEditor = TextEditingController();
  var PasswordEditor = TextEditingController();

  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    IdpAuthority = Get.find<IAppEnv>().idpAuthority;
    UserNameEditor.text = UserName;
  }

  isLoggedIn() {
    return Token != '' && Profile.UserId != '';
  }

  login(bool keepUserName) async {
    await Get.find<AuthService>()
        .login(UserNameEditor.text, PasswordEditor.text)
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
}
