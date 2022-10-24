# Configure the AWS Provider
data "aws_route53_zone" "selected" {
  name         = "metadlw.com"
}

resource "aws_route53_record" "admin" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = var.admin_record
  type    = "A"
  ttl     = 300
  records = [azurerm_public_ip.gwIp.ip_address]
}

resource "aws_route53_record" "customer" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = var.customer_record
  type    = "A"
  ttl     = 300
  records = [azurerm_public_ip.gwIp.ip_address]
}

resource "aws_route53_record" "admin_asuid" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = "asuid.${var.admin_record}"
  type    = "TXT"
  ttl     = 300
  records = [data.azurerm_linux_web_app.umbapp.custom_domain_verification_id]
}

resource "aws_route53_record" "customer_asuid" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = "asuid.${var.customer_record}"
  type    = "TXT"
  ttl     = 300
  records = [data.azurerm_linux_web_app.umbapp.custom_domain_verification_id]
}
