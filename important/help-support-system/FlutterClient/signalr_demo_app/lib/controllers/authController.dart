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
    UserNameEditor.text = await authStorageService.GetUserName();
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
