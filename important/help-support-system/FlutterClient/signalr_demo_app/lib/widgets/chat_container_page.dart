import 'package:flutter/material.dart';
import 'package:signalr_demo_app/models/chat_member.dart';
import 'package:signalr_demo_app/models/contact.dart';
import '../controllers/chatContainerController.dart';
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
                      height: 30,
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(30),
                        color: Colors.pink[50],
                      ),
                      child: Row(
                        children: <Widget>[
                          Icon(
                            Icons.add,
                            color: Colors.pink,
                            size: 20,
                          ),
                          SizedBox(
                            width: 2,
                          ),
                          Text(
                            "Add New",
                            style: TextStyle(
                                fontSize: 14, fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                    )
                  ],
                ),
              ),
            ),
            ListView.builder(
              itemCount: controller.UserContact.Groups.length,
              shrinkWrap: true,
              padding: EdgeInsets.only(top: 16),
              physics: NeverScrollableScrollPhysics(),
              itemBuilder: (context, index) {
                return ConversationItem(
                  profile: controller.Profile,
                  member: getChatMemberFromGroup(
                      controller.UserContact.Groups[index]),
                  isMessageRead: (index == 0 || index == 3) ? true : false,
                );
              },
            ),
            ListView.builder(
              itemCount: controller.UserContact.Friends.length,
              shrinkWrap: true,
              padding: EdgeInsets.only(top: 16),
              physics: NeverScrollableScrollPhysics(),
              itemBuilder: (context, index) {
                return ConversationItem(
                  profile: controller.Profile,
                  member: getChatMemberFromUser(
                      controller.UserContact.Friends[index]),
                  isMessageRead: (index == 0 || index == 3) ? true : false,
                );
              },
            ),
          ],
        ),
      ),
    );
  }

  getChatMemberFromUser(MemberInfo info) {
    var chatId = info.UserId;
    var summary = controller.getChatSummary(chatId);
    return ChatMember(
      talkingTo: chatId,
      name: info.UserName,
      messageText: summary.LatestMsge,
      imageURL:
          "https://cdn.pixabay.com/photo/2015/11/16/14/43/cat-1045782__340.jpg",
      unread: summary.UnreadCount,
      time: summary.Time.toString(),
      type: "user",
    );
  }

  getChatMemberFromGroup(GroupInfo info) {
    var chatId = info.GroupId;
    var summary = controller.getChatSummary(chatId);
    return ChatMember(
      talkingTo: chatId,
      name: info.Name,
      messageText: summary.LatestMsge,
      imageURL: "https://thumbs.dreamstime.com/z/little-cats-20284533.jpg",
      unread: summary.UnreadCount,
      time: summary.Time.toString(),
      type: "group",
    );
  }
}
