import {ILabelModel} from "./ILabelModel";

export interface IEventModel{
  eventId: number;
  eventDate: string;
  eventTitle: string;
  eventDescription: string;
  eventCreatedDate: string;
  labels?: ILabelModel[];
}
