import {ILabelModel} from "./ILabelModel";
import {IGroupModel} from "./IGroupModel";

export interface IEventModel{
  eventId: number;
  eventDate: string;
  eventTitle: string;
  eventDescription: string;
  eventCreatedDate: string;
  labels?: ILabelModel[];
  group: IGroupModel;
}
