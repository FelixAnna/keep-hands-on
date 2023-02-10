import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/profileController.dart';

class ProfilePage extends GetWidget<ProfileController> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(32),
        child: Row(
          children: [
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  /*2*/
                  Obx(
                    () => CircleAvatar(
                      backgroundImage: NetworkImage(controller
                                  .UserInfo.value.AvatarUrl ==
                              ''
                          ? "https://upload.wikimedia.org/wikipedia/commons/e/ee/Unknown-person.gif"
                          : controller.UserInfo.value.AvatarUrl),
                      maxRadius: 60,
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.only(bottom: 8),
                    child: Obx(() => Text(
                          controller.UserInfo.value.NickName,
                          style: TextStyle(
                            fontWeight: FontWeight.bold,
                          ),
                        )),
                  ),
                  Obx(() => Text(
                        controller.UserInfo.value.Email,
                        style: TextStyle(
                          color: Colors.grey[500],
                        ),
                      )),
                  Obx(() => Text(
                        controller.UserInfo.value.UserId,
                        style: TextStyle(
                          color: Colors.grey[500],
                        ),
                      )),
                  Obx(() => Text(
                        controller.TenantName.value,
                        style: TextStyle(
                          color: Colors.grey[500],
                        ),
                      )),
                ],
              ),
            ),
            /*3*/
            Icon(
              Icons.star,
              color: Colors.amber,
            ),
            const Text('99'),
          ],
        ),
      ),
    );
  }
}
