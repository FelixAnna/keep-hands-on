class ChatMessage {
  int id;
  String sender;
  String messageContent;
  String messageType;
  ChatMessage(
      {required this.id,
      required this.sender,
      required this.messageContent,
      required this.messageType});
}
