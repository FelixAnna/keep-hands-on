import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/baseController.dart';
import 'package:signalr_demo_app/services/user_service.dart';

class SearchUserController extends BaseController {
  var Users = [].obs;

  late UserService userService;

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
  }
}
