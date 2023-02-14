import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/models/contact.dart';
import 'package:signalr_demo_app/widgets/group_item.dart';
import 'package:signalr_demo_app/widgets/search_user_page.dart';
import 'package:signalr_demo_app/widgets/user_item.dart';
import '../controllers/contactController.dart';
import '../controllers/searchUserController.dart';

class ContactPage extends GetWidget<ContactController> {
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
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
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
                      "Groups & Friends",
                      style:
                          TextStyle(fontSize: 23, fontWeight: FontWeight.bold),
                    ),
                    Container(
                      padding:
                          EdgeInsets.only(left: 8, right: 8, top: 2, bottom: 2),
                      height: 36,
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(30),
                        color: Colors.lightBlue[50],
                      ),
                      child: Row(
                        children: <Widget>[
                          new IconButton(
                            icon: new Icon(Icons.person_add),
                            color: Colors.lightBlue,
                            onPressed: () async => {
                              if (!Get.isRegistered<SearchUserController>())
                                {
                                  Get.put(SearchUserController(),
                                      permanent: true),
                                },
                              await Get.find<SearchUserController>().initial(),
                              Get.to(() => SearchUserPage()),
                            },
                          ),
                          SizedBox(
                            width: 2,
                          )
                        ],
                      ),
                    )
                  ],
                ),
              ),
            ),
            Obx(() => ListView.builder(
                  itemCount: controller.UserContact.value.Groups.length,
                  shrinkWrap: true,
                  padding: EdgeInsets.only(top: 16),
                  physics: NeverScrollableScrollPhysics(),
                  itemBuilder: (context, index) {
                    var info = controller.UserContact.value.Groups[index];
                    return GroupItem(
                      member: info,
                    );
                  },
                )),
            Obx(() => ListView.builder(
                  itemCount: controller.UserContact.value.Friends.length,
                  shrinkWrap: true,
                  padding: EdgeInsets.only(top: 16),
                  physics: NeverScrollableScrollPhysics(),
                  itemBuilder: (context, index) {
                    var info = controller.UserContact.value.Friends[index];
                    return UserItem(
                      member: new ColleagueInfo(
                        AvatarUrl: info.AvatarUrl,
                        UserId: info.UserId,
                        IsFriend: true,
                        Email: info.Email,
                        TenantId: info.TenantId,
                        NickName: info.NickName,
                      ),
                    );
                  },
                ))
          ],
        ),
      ),
    );
  }
}
