import 'package:get/get.dart';
import '../models/user.dart';
import 'baseController.dart';

class ProfileController extends BaseController {
  var UserInfo = User(UserId: '', NickName: '', AvatarUrl: '', Email: '').obs;
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
  }
}
