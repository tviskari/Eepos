variable "project_name" {
  description = "Projektin nimi, käytetään resurssien nimeämisessä"
  type        = string
  default     = "eepos"
}

variable "environment" {
  description = "Ympäristö (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "location" {
  description = "Azure-alue"
  type        = string
  default     = "northeurope"
}

variable "image_tag" {
  description = "Docker-imagen tagi"
  type        = string
  default     = "latest"
}
