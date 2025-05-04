
import { faArrowLeft, faArrowRight } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Carousel } from "antd";
import { FC, useEffect, useRef, useState } from "react";
import { SlideShowResource } from "../../resources";
import settingService from "../../services/setting-service";


type SliderProps = {
}

const Slider: FC<SliderProps> = ({
}): JSX.Element => {
    const [onHover, setOnHover] = useState(false)
    const [currentSlide, setCurrentSlide] = useState(0);
    const [slideShows, setSlideShows] = useState<SlideShowResource[]>([]);
    const [totalSlides, setTotalSlides] = useState(0)

    const fetchSlideShows = async () => {
        const response = await settingService.getAllSlideShows();
        setSlideShows(response.data)
        setTotalSlides(response.data.length ?? 0)
    }
    useEffect(() => {
        fetchSlideShows();
    }, [])



    const carouselRef = useRef<any>(null);

    const handleNext = () => {
        if (currentSlide < totalSlides - 1) {
            carouselRef.current.next();
        }
    };

    const handlePrev = () => {
        if (currentSlide > 0) {
            carouselRef.current.prev();
        }
    };


    return <div onMouseLeave={() => setOnHover(false)} onMouseOver={() => setOnHover(true)} className="absolute top-0 left-0 right-0 bottom-0">
        <Carousel autoplay autoplaySpeed={2000} style={{
        }} afterChange={(currentIndex) => {
            setCurrentSlide(currentIndex)
        }} draggable ref={carouselRef} infinite={false}>

            {slideShows.map((slide, index) => <div key={slide.id}>
                <div className="flex justify-start z-5 h-[90vh] relative" >
                    <div className="absolute top-0 right-0 bottom-1/2 lg:bottom-0 left-0" style={{
                        margin: 0,
                        color: '#fff',
                        backgroundImage: `
                        url(${slide.backgroundImage})
                    `,
                        backgroundSize: 'cover',
                        backgroundPosition: 'center'
                    }}>

                    </div>
                    <div className="flex flex-col w-full lg:w-1/2 justify-center items-center lg:items-start p-14 gap-y-6 lg:static absolute top-1/2 left-0 right-0 bottom-0">
                        <span className={`${currentSlide == index ? 'opacity-100 translate-y-0' : 'opacity-0 -translate-y-10'} transition-transform ease-out duration-1000 lg:text-left text-2xl lg:text-5xl font-semibold text-center`}>{slide.title}</span>
                        <p className={`${currentSlide == index ? 'opacity-100 translate-y-0' : 'opacity-0 -translate-y-4'} transition-transform ease-out duration-700 text-left text-lg hidden lg:block`}>{slide.description}</p>
                        <button className={`${currentSlide == index ? 'opacity-100 translate-y-0' : 'opacity-80 -translate-y-2'} transition-transform ease-out duration-700 text-lg bg-white px-10 py-3 rounded-3xl text-primary border-primary border-[1px] shadow-sm hover:bg-primary hover:text-white`} >{slide.btnTitle}</button>
                    </div>
                </div>
            </div>)}


        </Carousel>
        <button onClick={handlePrev} className={`bg-black ${currentSlide === 0 ? 'bg-opacity-40' : 'bg-opacity-85'} absolute top-1/2 -translate-y-1/2 transition-all ease-in-out ${onHover ? 'left-5 opacity-100' : 'left-0 opacity-0'} w-12 h-12 rounded-full flex justify-center text-white items-center`}><FontAwesomeIcon icon={faArrowLeft} /></button>
        <button onClick={handleNext} className={`bg-black ${currentSlide === totalSlides - 1 ? 'bg-opacity-40' : 'bg-opacity-85'} absolute top-1/2 -translate-y-1/2 transition-all ease-in-out ${onHover ? 'right-5 opacity-100' : 'right-0 opacity-0'} w-12 h-12 rounded-full flex justify-center text-white items-center`}><FontAwesomeIcon icon={faArrowRight} /></button>
    </div>
};

export default Slider;
