class User {
  String UserId;
  String UserName;
  String Email;

  User({required this.UserId, required this.UserName, required this.Email});

  User.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        UserName = json['userName'],
        Email = json['email'];

  Map<String, dynamic> toJson() =>
      {'userId': this.UserId, 'userName': this.UserName, 'email': this.Email};
}
