import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/widgets/user_item.dart';
import '../controllers/searchUserController.dart';

class SearchUserPage extends GetWidget<SearchUserController> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        physics: BouncingScrollPhysics(),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            SafeArea(
              child: Padding(
                padding: EdgeInsets.only(left: 16, right: 16, top: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.start,
                  children: <Widget>[
                    IconButton(
                      onPressed: () {
                        Get.back();
                      },
                      icon: Icon(
                        Icons.arrow_back,
                        color: Colors.black,
                      ),
                    ),
                    Text(
                      "Search User",
                      style:
                          TextStyle(fontSize: 23, fontWeight: FontWeight.bold),
                    )
                  ],
                ),
              ),
            ),
            CupertinoSearchTextField(
              controller: controller.textController,
              onChanged: (value) async {
                await controller.Search(value);
              },
              onSubmitted: (value) async {
                await controller.Search(value);
              },
              autocorrect: true,
            ),
            Obx(() => ListView.builder(
                  itemCount: controller.Users.length,
                  shrinkWrap: true,
                  padding: EdgeInsets.only(top: 16),
                  physics: NeverScrollableScrollPhysics(),
                  itemBuilder: (context, index) {
                    var info = controller.Users[index];
                    return UserItem(
                      member: info,
                    );
                  },
                )),
          ],
        ),
      ),
    );
  }
}
