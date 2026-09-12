import { NotificationType } from './enums';

export interface NotificationDto {
  id: number;
  title: string;
  message: string;
  type: NotificationType;
  read: boolean;
  createdAt: string;
}
