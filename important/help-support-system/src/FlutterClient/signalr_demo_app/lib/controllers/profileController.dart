import 'dart:convert';

import 'package:get/get.dart';
import '../models/user.dart';
import 'baseController.dart';

class ProfileController extends BaseController {
  var UserInfo =
      User(UserId: '', TenantId: -1, NickName: '', AvatarUrl: '', Email: '')
          .obs;
  var TenantName = "".obs;
  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    if (!await loadUserFromCache()) {
      Get.toNamed("/login");
      return;
    }

    UserInfo.value = Profile;

    var tenants = await loadTenants();
    //print(tenants);
    if (tenants != '') {
      var tenant = jsonDecode(tenants)
          .where((element) => element["id"] == Profile.TenantId);
      TenantName.value = tenant.last["name"];
    }
  }
}
