import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/baseController.dart';
import 'package:signalr_demo_app/services/user_service.dart';

import '../models/contact.dart';

class ContactController extends BaseController {
  var UserContact = Contact("", [], []).obs;
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

    //load users
    userService = Get.find<UserService>();
    await loadContactAndMessages();
  }

  loadContactAndMessages() async {
    userService = Get.find();
    var contact = await userService.getCurrentContacts();

    // initial contact
    UserContact.value = contact;
  }
}
