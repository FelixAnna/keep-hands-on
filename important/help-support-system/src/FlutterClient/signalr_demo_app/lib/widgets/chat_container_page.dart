import 'package:flutter/material.dart';
import 'package:signalr_demo_app/controllers/searchUserController.dart';
import 'package:signalr_demo_app/models/chat_member.dart';
import 'package:signalr_demo_app/widgets/search_user_page.dart';
import '../controllers/chatContainerController.dart';
import '../controllers/contactController.dart';
import 'contact_page.dart';
import 'conversation_item.dart';
import 'package:get/get.dart';

class ChatContainerPage extends GetWidget<ChatContainerController> {
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
                    Text(
                      "Conversations",
                      style:
                          TextStyle(fontSize: 32, fontWeight: FontWeight.bold),
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
                            icon: new Icon(Icons.people),
                            color: Colors.lightBlue,
                            onPressed: () async => {
                              if (!Get.isRegistered<ContactController>())
                                {
                                  Get.put(ContactController(), permanent: true),
                                },
                              await Get.find<ContactController>().initial(),
                              Get.to(() => ContactPage()),
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
                    var chatId = info.GroupId;
                    return Obx(() => ConversationItem(
                          profile: controller.Profile,
                          member: ChatMember(
                            talkingTo: chatId,
                            name: info.Name,
                            messageText:
                                controller.ChatMsgs[chatId]!.LatestMsge,
                            imageURL:
                                "https://thumbs.dreamstime.com/z/little-cats-20284533.jpg",
                            unread: controller.ChatMsgs[chatId]!.UnreadCount,
                            time: getTime(controller.ChatMsgs[chatId]!.Time),
                            type: "group",
                          ),
                          //getChatMemberFromGroup(
                          //  controller.UserContact.value.Groups[index]),
                          isMessageRead:
                              (index == 0 || index == 3) ? true : false,
                        ));
                  },
                )),
            Obx(() => ListView.builder(
                  itemCount: controller.UserContact.value.Friends.length,
                  shrinkWrap: true,
                  padding: EdgeInsets.only(top: 16),
                  physics: NeverScrollableScrollPhysics(),
                  itemBuilder: (context, index) {
                    var info = controller.UserContact.value.Friends[index];
                    var chatId = info.UserId;
                    return Obx(() => ConversationItem(
                          profile: controller.Profile,
                          member: ChatMember(
                            talkingTo: chatId,
                            name: info.NickName,
                            messageText:
                                controller.ChatMsgs[chatId]!.LatestMsge,
                            imageURL: info.AvatarUrl,
                            unread: controller.ChatMsgs[chatId]!.UnreadCount,
                            time: getTime(controller.ChatMsgs[chatId]!.Time),
                            type: "user",
                          ),
                          //getChatMemberFromUser(
                          //  controller.UserContact.value.Friends[index]),
                          isMessageRead:
                              (index == 0 || index == 3) ? true : false,
                        ));
                  },
                )),
          ],
        ),
      ),
    );
  }

  String getTime(DateTime? time) {
    if (time == null) {
      return "Never";
    }

    var now = DateTime.now().toUtc();
    var diff = now.difference(time).inMinutes;
    if (diff <= 2) {
      return "Just now";
    } else if (diff < 60) {
      return "${diff} mins ago";
    } else if (diff < 1440) {
      return "${(diff / 60).round()} hour(s) ago";
    } else {
      return time.toIso8601String();
    }
  }
}
