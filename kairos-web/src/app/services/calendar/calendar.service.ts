import { Injectable } from '@angular/core';
import { IEventModel } from 'src/app/models/ievent.model';


@Injectable({
  providedIn: 'root'
})
export class CalendarService {

  private nextEventId: number = 128;
  private events: IEventModel[] = [
    { id: 1, title: 'Etudier les logarithmes', description: 'Test logarithmes 16 novembre, revoir les règles de simplification et refaire les exercices', label: 'Math', sessionDate: '15.11.2022' },
    { id: 2, title: 'Test Economie', description: 'Test économie sur les systèmes économique', label: 'Economie', sessionDate: '22.11.2022' },
    { id: 3, title: 'Apprendre vocabulaire d\'anglais', description: 'Réviser vobulaire de l\'Unit 6 pour le test', label: 'Anglais', sessionDate: '22.11.2022' },
    { id: 4, title: 'Lorem ipsum', description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam malesuada libero odio, a mattis augue efficitur vel. Cras in lectus at eros porta sagittis. Morbi eu libero tortor. Phasellus in congue lacus. Duis mollis, orci at facilisis tempus, urna libero cursus sapien, in interdum lacus tortor eget neque. Nullam metus leo, viverra sit amet imperdiet et, mattis eu dolor.', label: 'Lorem ipsum', sessionDate: '01.01.1901' },
    { id: 5, title: 'Lorem ipsum', description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam malesuada libero odio, a mattis augue efficitur vel. Cras in lectus at eros porta sagittis. Morbi eu libero tortor.', label: 'Lorem ipsum', sessionDate: '01.01.1901' }
  ];

  constructor() { }

  getEvents(): IEventModel[] {
    return this.events;
  }

  getEvent(eventId: number): IEventModel|undefined {
    return this.events.find((event: IEventModel) => event.id === eventId);
  }

  createEvent(): IEventModel {
    const id: number = this.getNextEventId();
    const event: IEventModel = {
      id,
      title: 'Event #' + id
    };
    this.events.push(event);
    
    return event;
  }

  deleteEvent(event: IEventModel): void {
    this.events = this.events.filter((item: IEventModel) => item.id !== event.id);
  }

  saveEvent(event: IEventModel): void {
    // si la event existe dans le tableau
    const eventIndex: number = this.events.indexOf(event);
    eventIndex >= 0 ?
      this.events[eventIndex] = event :  // on la met à jour
      this.events.push(event);          // sinon on l'ajoute
  }

  private getNextEventId(): number {
    const id = this.nextEventId;
    this.nextEventId++;
    return id;
  }
}