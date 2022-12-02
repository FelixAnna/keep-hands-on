class ChatMember {
  String talkingTo;
  String name;
  String messageText;
  String imageURL;
  String time;
  String type; // "group" , "user"
  ChatMember(
      {required this.talkingTo,
      required this.name,
      required this.messageText,
      required this.imageURL,
      required this.time,
      required this.type});
}
