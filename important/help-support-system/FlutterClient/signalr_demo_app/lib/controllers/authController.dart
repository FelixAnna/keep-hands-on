import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/config/env.dart';
import 'package:signalr_demo_app/controllers/baseController.dart';
import 'package:signalr_demo_app/models/user.dart';
import 'package:signalr_demo_app/services/auth_service.dart';

class AuthController extends BaseController {
  late String IdpAuthority = '';

  var UserNameEditor = TextEditingController();
  var PasswordEditor = TextEditingController();

  var isLoading = false.obs;

  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    IdpAuthority = Get.find<IAppEnv>().idpAuthority;
    await loadUserFromCache();
    UserNameEditor.text = UserName;
  }

  login(bool keepUserName) async {
    var userName = UserNameEditor.text;
    var password = PasswordEditor.text;

    if (userName == '' || password == '') {
      return;
    }

    isLoading.value = true;
    await Get.find<AuthService>()
        .login(userName, password)
        .then((response) async => {
              Token = response["accessToken"],
              Profile = User.fromJson(response["profile"]),
              await authStorageService.SaveLoginResult(response, keepUserName),
            })
        .onError((error, stackTrace) => {
              Token = '',
            });
    isLoading.value = false;
    update();
  }
}
