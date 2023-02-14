import 'dart:convert';

import 'package:get/get.dart';

import '../models/user.dart';
import '../services/auth_service.dart';
import '../services/auth_storage_service.dart';
import '../services/hub_service.dart';
import '../services/tenant_service.dart';

class BaseController extends GetxController {
  late String UserName = "";
  late String Token = "";
  late User Profile =
      User(UserId: '', TenantId: -1, NickName: '', AvatarUrl: '', Email: '');

  late AuthStorageService authStorageService;
  @override
  void onInit() async {
    super.onInit();
  }

  Future<bool> loadUserFromCache() async {
    // load last login info from local cache
    authStorageService = Get.find<AuthStorageService>();
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
    await authStorageService.ClearLoginResult();
    await authStorageService.ClearTenantsResult();
    await hubService.stop();
  }

  loadTenants() async {
    var service = Get.find<AuthStorageService>();
    var tenants = await service.GetTenantsCache();
    //print(tenants);
    if (tenants != '') {
      return tenants;
    }

    await Get.find<TenantService>()
        .loadTenants("Ready")
        .then((response) async => {
              await service.SaveTenantsResult(response),
            })
        .onError((error, stackTrace) => {
              Token = '',
            });

    tenants = await service.GetTenantsCache();
    return tenants;
  }
}
