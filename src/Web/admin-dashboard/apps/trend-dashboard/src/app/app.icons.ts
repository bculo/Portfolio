import { 
    heroNewspaper, 
    heroHome, 
    heroChevronRight, 
    heroChatBubbleLeftEllipsis, 
    heroPower,
    heroDocumentPlus,
    heroCog6Tooth,
    heroTrash,
    heroPlusCircle ,
    heroCalendarDays,
    heroMagnifyingGlassCircle,
    heroBackward
} from '@ng-icons/heroicons/outline';

import {
    MoveDirection,
    OutMode,
  } from "@tsparticles/engine";

export const APP_ICONS = {
    heroNewspaper,
    heroHome,
    heroChevronRight,
    heroChatBubbleLeftEllipsis,
    heroPower,
    heroDocumentPlus,
    heroCog6Tooth,
    heroTrash,
    heroPlusCircle,
    heroCalendarDays,
    heroMagnifyingGlassCircle,
    heroBackward
}

export const ANIMATED_BACKGROUND = {
    background: {
      color: {
        value: "#111827",
      },
    },
    fpsLimit: 120,
    interactivity: {
      modes: {
        push: {
          quantity: 4,
        },
        repulse: {
          distance: 200,
          duration: 0.4,
        },
      },
    },
    particles: {
      color: {
        value: "#ffffff",
      },
      links: {
        color: "#ffffff",
        distance: 150,
        enable: true,
        opacity: 0.5,
        width: 1,
      },
      move: {
        direction: MoveDirection.none,
        enable: true,
        outModes: {
          default: OutMode.bounce,
        },
        random: false,
        speed: 2,
        straight: false,
      },
      number: {
        density: {
          enable: true,
          area: 100,
        },
        value: 49,
      },
      opacity: {
        value: 0.3,
      },
      shape: {
        type: "circle",
      },
      size: {
        value: { min: 1, max: 3 },
      },
    },
    detectRetina: true,
  };