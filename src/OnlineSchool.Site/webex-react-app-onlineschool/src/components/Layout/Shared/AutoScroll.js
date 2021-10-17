import { useEffect } from "react";

const AutoScroll = () => {
    
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  return null;
}

export default AutoScroll;