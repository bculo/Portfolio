export const BackgroundPattern = () => {
  return (
    <div className="w-full h-full absolute top-0 left-0 -z-50">
      <svg
        id="patternId"
        width="100%"
        height="100%"
        xmlns="http://www.w3.org/2000/svg"
      >
        <defs>
          <pattern
            id="a"
            patternUnits="userSpaceOnUse"
            width="25.5"
            height="26"
            patternTransform="scale(5) rotate(20)"
          >
            <rect
              x="0"
              y="0"
              width="100%"
              height="100%"
              fill="hsla(0, 0%, 100%, 0)"
            />
            <path
              d="M10-6V6M10 14v12M26 10H14M6 10H-6"
              transform="translate(2.75,0)"
              stroke="hsla(259, 46%, 51%, 0.06)"
              fill="none"
            />
          </pattern>
        </defs>
        <rect
          width="800%"
          height="800%"
          transform="translate(-175,0)"
          fill="url(#a)"
        />
      </svg>
    </div>
  );
};
